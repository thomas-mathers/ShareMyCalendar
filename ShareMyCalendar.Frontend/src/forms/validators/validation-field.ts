interface ValidationField {
    label: string;
    value: string;
}

type Validator = (field: ValidationField) => string;

export default ValidationField;
export type { Validator }