# Building Forms with Validation

Blazor offers the following components

- EditForm
- InputBase<>
- InputCheckbox
- InputDate<TValue>
- InputNumber<TValue>
- InputSelect<TValue>
- InputText
- InputTextArea
- InputRadio
- InputRadioGroup
- ValidationMessage
- ValidationSummary

## EditForm

EditForm will create an EditContext instance as a cascading value so that all the components you put inside of EditForm will access the same EditContext. EditContext will track the meta data regarding the editing process, such as what fields have been edited, and keep track of any validation messages.

You need to assign either a model (a class you wish to edit) or an EditContext instance.
For most use cases, assigning a model is the way to go, but for more advanced scenarios, you might want to be able to trigger EditContext.Validate(), for example, to validate all the controls connected to EditContext.

EditForm has the following events

- OnValidSubmit gets triggered when the data in the form validates correctly
- OnInvalidSubmit gets triggered if the form does not validate correctly
- OnSubmit gets triggered when the form is submitted, regardless of whether the form validates correctly or not. Use OnSubmit if you want to control the validation yourself.

## InputBase<>

