# SuperBehaviour Documentation

For a complete and up-to-date documentation, please visit our [**Documentation Website**](https://superbehaviour.lom.li/).

## References
> **Tutorial** [https://superbehaviour.lom.li/tutorials](https://superbehaviour.lom.li/tutorials)

> **Detailed Documentation** [https://superbehaviour.lom.li/api](https://superbehaviour.lom.li/api)

> **Discord Server** [https://discord.gg/93fUfVbFQw](https://discord.gg/93fUfVbFQw)

## Short Offline Documentation
Most important classes are:

| Class | Description | Namespace |
| --- | --- | ---: |
| [SuperBehaviour](https://superbehaviour.lom.li/LoM.Super.SuperBehaviour.html) | Replaces the default MonoBehaviour and enables you to use all the features of this package. | LoM.Super |
| [SingletonBehaviour<T>](https://superbehaviour.lom.li/LoM.Super.SingletonBehaviour-1.html) | Implements the Singleton pattern (Based on SuperBehaviour) | LoM.Super |
| [LazySingletonBehaviour<T>](https://superbehaviour.lom.li/LoM.Super.LazySingletonBehaviour-1.html) | Implements the lazy Singleton pattern, which will create an instance of the gameobject if not present in the scene at the time of .Instance call. (Based on SuperBehaviour) | LoM.Super |
| [SerializePropertyAttribute](https://superbehaviour.lom.li/LoM.Super.SerializePropertyAttribute.html) | Attribute to mark properties to be shown in the inspector. (Properties are read only unless you are in play mode because it will not be saved or serialized in any way) | LoM.Super |
| [SuperEditor<T>](https://superbehaviour.lom.li/LoM.Super.Editor.SuperEditor-1.html) | Replaces the default Editor and enables you to use all the features of this package. | LoM.Super.Editor |
| [SuperPropertyDrawer](https://superbehaviour.lom.li/LoM.Super.Editor.Drawer.SuperPropertyDrawer.html) | Replaces the default PropertyDrawer uses SuperSerializedProperty wich is able to draw fields and properties in the inspector. | LoM.Super.Editor.Drawer |

List of all Attributes:

| Attribute | Fields | Properties | Description |
| --- | --- | --- | --- |
| EditableIf | [ [EditableIf](https://superbehaviour.lom.li/LoM.Super.EditableIfAttribute.html)("FieldName") ] | [ [EditableIf](https://superbehaviour.lom.li/LoM.Super.EditableIfAttribute.html)("FieldName") ] | Enables you to make a field editable in the inspector if a condition is met. (If target is not a Boolean it checkts for NULL) |
| ShowIf | [ [ShowIf](https://superbehaviour.lom.li/LoM.Super.ShowIfAttribute.html)("FieldName") ] | [ [ShowIf](https://superbehaviour.lom.li/LoM.Super.ShowIfAttribute.html)("FieldName")  ] | Enables you to show a field in the inspector if a condition is met. (If target is not a Boolean it checkts for NULL) |
| Label | [ [Label](https://superbehaviour.lom.li/LoM.Super.LabelAttribute.html)("Label") ] | [ [Label](https://superbehaviour.lom.li/LoM.Super.LabelAttribute.html)("Label") ] | Enables you to change the label of a field in the inspector. |
| ReadOnly | [ [ReadOnly](https://superbehaviour.lom.li/LoM.Super.ReadOnlyAttribute.html) ] | [ [ReadOnly](https://superbehaviour.lom.li/LoM.Super.ReadOnlyAttribute.html) ] | Makes a field read only in the inspector. |
| HDRColor | [ [HDRColor](https://superbehaviour.lom.li/LoM.Super.HDRColorAttribute.html) ] | [ [PropertyHDRColor](https://superbehaviour.lom.li/LoM.Super.PropertyHDRColorAttribute.html) ] | Display Color fields in the inspector as HDR Color. |
| Layer | [ [Layer](https://superbehaviour.lom.li/LoM.Super.LayerAttribute.html) ] | [ [PropertyLayer](https://superbehaviour.lom.li/LoM.Super.PropertyLayerAttribute.html) ] | Display int fields in the inspector as Layer. |
| Tag | [ [Tag](https://superbehaviour.lom.li/LoM.Super.TagAttribute.html) ] | [ [PropertyTag](https://superbehaviour.lom.li/LoM.Super.PropertyTagAttribute.html) ] | Display string fields in the inspector as Tag. |
| Header | [ [Header](https://docs.unity3d.com/ScriptReference/HeaderAttribute.html)("Text") ] | [ [PropertyHeader](https://superbehaviour.lom.li/LoM.Super.PropertyHeaderAttribute.html)("Text") ] | Adds a header to the inspector. |
| Range | [ [Range](https://docs.unity3d.com/ScriptReference/RangeAttribute.html)(1, 6) ] | [ [PropertyRange](https://superbehaviour.lom.li/LoM.Super.PropertyRangeAttribute.html)(1, 6) ] | Display int and float fields in the inspector as a slider. |
| Space | [ [Space](https://docs.unity3d.com/ScriptReference/SpaceAttribute.html) ] | [ [PropertySpace](https://superbehaviour.lom.li/LoM.Super.PropertySpaceAttribute.html) ] | Adds a space to the inspector. |
| Tooltip | [ [Tooltip](https://docs.unity3d.com/ScriptReference/TooltipAttribute.html)("Text") ] | [ [PropertyTooltip](https://superbehaviour.lom.li/LoM.Super.PropertyTooltipAttribute.html)("Text") ] | Adds a tooltip to the inspector. |
| TextArea | [ [TextArea](https://docs.unity3d.com/ScriptReference/TextAreaAttribute.html) ] | [ [PropertyTextArea](https://superbehaviour.lom.li/LoM.Super.PropertyTextAreaAttribute.html) ] | Display string fields in the inspector as TextArea. |