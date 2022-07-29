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

export { FieldType };
export type { Field }
