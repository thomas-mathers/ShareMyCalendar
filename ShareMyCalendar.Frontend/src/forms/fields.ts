import { TextFieldProps } from '@mui/material';
import { Validator } from './validators/validation-field';

enum FieldType {
    Text,
    Checkbox,
    Select
}

interface TextFormField {
    type: FieldType.Text;
    label: string;
    value: string;
    validators: Validator[];
    errorMessages: string[];
    textFieldProps?: TextFieldProps;
}
interface CheckboxFormField {
    type: FieldType.Checkbox;
    label: string;
    value: string;
    validators: Validator[];
    errorMessages: string[];
}
interface SelectFormField {
    type: FieldType.Select;
    label: string;
    value: string;
    options: string[];
    validators: Validator[];
    errorMessages: string[];
}

type Field = TextFormField | CheckboxFormField | SelectFormField;

function isValid(field: Field) {
    for (let i = 0; i < field.validators.length; i++) {
        const errorMessage = field.validators[i](field);

        if (errorMessage) {
            return false;
        }
    }
    return true;
}

function validate(field: Field) {
    field.errorMessages = [];

    for (let i = 0; i < field.validators.length; i++) {
        const errorMessage = field.validators[i](field);

        if (errorMessage) {
            field.errorMessages.push(errorMessage);
        }
    }
}

export { FieldType, isValid, validate };
export type { Field }
