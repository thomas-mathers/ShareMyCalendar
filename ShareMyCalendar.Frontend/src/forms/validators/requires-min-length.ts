import ValidationField from "./validation-field";

function hasMinLength(value: string, length: number) {
    return value.length >= length;
}

function requiresMinLength(length: number) {
    return (field: ValidationField) => hasMinLength(field.value, length)
        ? ''
        : `${field.label} must be at least ${length} characters long`;
} 

export default requiresMinLength;
export { hasMinLength };