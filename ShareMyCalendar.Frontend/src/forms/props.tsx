import { TextFieldProps } from '@mui/material';
import { Validator } from './validators/validation-field';
import { FieldType } from './fields';
import BinaryConstraint from "./binary-constraint";

interface TextFormFieldProps {
    type: FieldType.Text;
    label: string;
    value?: string;
    validators?: Validator[];
    textFieldProps?: TextFieldProps;
}
interface CheckboxFormFieldProps {
    type: FieldType.Checkbox;
    label: string;
    value?: string;
    validators?: Validator[];
}
interface SelectFormFieldProps {
    type: FieldType.Select;
    label: string;
    value?: string;
    options: string[];
    validators: Validator[];
}

type FieldProps = TextFormFieldProps | CheckboxFormFieldProps | SelectFormFieldProps;

interface Props {
    fields: Record<string, FieldProps>;
    constraints: BinaryConstraint[];
}

export default Props;
