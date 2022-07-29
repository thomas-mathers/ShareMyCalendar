import ValidationField from "./validation-field";

function isTruthy(value: string): boolean {
    return !!value;
}

function required(field: ValidationField) {
    return isTruthy(field.value)
        ? ''
        : `${field.label} is required`;
}

export default required;
export { isTruthy }