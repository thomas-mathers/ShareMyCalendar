import { State } from "./state";
import { Field, FieldType } from "./fields";
import Props from "./props";

function initializeStateFromProps(props: Props): State {
    const fields: Record<string, Field> = {};
    for (const [k, v] of Object.entries(props.fields)) {
        switch (v.type) {
            case FieldType.Text:
                fields[k] = {
                    type: v.type,
                    label: v.label,
                    value: v.value ?? '',
                    validators: v.validators ?? [],
                    errorMessages: [],
                    textFieldProps: v.textFieldProps
                };
                break;
            case FieldType.Checkbox:
                fields[k] = {
                    type: v.type,
                    label: v.label,
                    value: v.value ?? '',
                    validators: v.validators ?? [],
                    errorMessages: [],
                };
                break;
            case FieldType.Select:
                fields[k] = {
                    type: v.type,
                    label: v.label,
                    value: v.value ?? '',
                    options: v.options,
                    validators: v.validators ?? [],
                    errorMessages: [],
                };
        }
    }
    return { fields, constraints: props.constraints };
}

export default initializeStateFromProps;