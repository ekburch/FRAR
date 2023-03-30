#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[CustomPropertyDrawer(typeof(ClassReferenceAttribute))]
public class ClassReferenceDrawer : PropertyDrawer
{
    //////////////////////////////////////////////////////////////////////////
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            // draw default
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height),
                property, true);
            return;
        }

        // update selected
        Type selected = null;
        var id = GUIUtility.GetControlID(FocusType.Passive);
        ObjectPickerWindow.TryGetPickedObject(id, ref selected);
        if (selected != null)
        {
            property.managedReferenceValue = selected == typeof(object) ? null : Activator.CreateInstance((Type)selected);
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        var testProperty = property;
        var propertyType = GetPropertyType(property);
        //var assembly = Assembly.GetExecutingAssembly();
        var assembly = Assembly.GetAssembly(propertyType);
        // draw select type btn
        if (GUI.Button(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
            getClassName(property.managedReferenceFullTypename)))
        {
            // object type means null value
            var typeList = implGetTypeList();
            typeList.Insert(0, typeof(object));

            ObjectPickerWindow.Show(id, null, typeList, 0, n => n == typeof(object) ? new GUIContent("Null") : new GUIContent(n.FullName));
        }
        // in header rect
        if (Event.current.type == EventType.ContextClick
            && new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight)
                .Contains(Event.current.mousePosition))
        {
            // create context menu
            var context = new GenericMenu();

            // null option
            context.AddItem(new GUIContent("Null"), false, () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            });

            // fill context menu types
            foreach (var type in implGetTypeList())
            {
                context.AddItem(new GUIContent(type.FullName), false, () => implSetProperty(type));
            }

            // show context window
            context.ShowAsContext();
        }

        // draw default
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height),
            property, true);


        /////////////////////////////////////
        // local functions

        // get class name function
        string getClassName(string managedReferenceFullTypename)
        {
            var result = "Null";
            if (string.IsNullOrEmpty(managedReferenceFullTypename) == false)
                result = property.managedReferenceFullTypename.Remove(0, assembly.FullName.Split(',')[0].Length);
            return result;
        }

        void filterTypes(Assembly asm, List<Type> output)
        {
            Type fieldType = null;

            // is list field
            if (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                fieldType = fieldInfo.FieldType.GetGenericArguments()[0];
            else
            // is array
            if (fieldInfo.FieldType.IsArray)
                fieldType = fieldInfo.FieldType.GetElementType();
            else
                // default field
                fieldType = fieldInfo.FieldType;

            Type[] types = asm.GetTypes();
            foreach (var type in asm.GetTypes())
            {
                if (type.IsVisible == false)
                    continue;

                if (type.IsClass == false)
                    continue;

                if (type.IsGenericType)
                    continue;

                if (type.IsAbstract)
                    continue;

                if (type.IsSerializable == false)
                    continue;

                if (fieldType.IsAssignableFrom(type) == false)
                    continue;

                output.Add(type);
            }
        }

        List<Type> implGetTypeList()
        {
            var typeList = new List<Type>();
            filterTypes(assembly, typeList);
            typeList.Sort((x, y) => string.CompareOrdinal(x.FullName, y.FullName));
            return typeList;
        }

        void implSetProperty(Type type)
        {
            property.managedReferenceValue = Activator.CreateInstance(type);
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

    public Type GetPropertyType(SerializedProperty property)
    {
        string referenceFieldTypeName = property.managedReferenceFieldTypename;
        Type actualType = GetTypeByName(referenceFieldTypeName);
        var type = actualType;
        return type;
    }

    public static Type GetTypeByName(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            return null;

        var typeSplit = typeName.Split(char.Parse(" "));
        var typeAssembly = typeSplit[0];
        var typeClass = typeSplit[1];

        return Type.GetType(typeClass + ", " + typeAssembly);
    }
}

#endif
