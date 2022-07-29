import ValidationField from "./validation-field";

function isUpper(c: string): boolean {
    return c.toUpperCase() === c && c !== c.toLowerCase();
}

function hasUpper(value: string): boolean {
    for (const c of value) {
        if (isUpper(c)) {
            return true;
        }
    }
    return false;
}

function requiresUpper(field: ValidationField) {
    return hasUpper(field.value)
        ? ''
        : `${field.label} requires an uppercase letter`;
} 

export default requiresUpper;
export { isUpper, hasUpper };