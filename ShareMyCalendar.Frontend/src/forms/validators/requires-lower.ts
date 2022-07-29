import ValidationField from "./validation-field";

function isLower(c: string): boolean {
    return c.toLowerCase() === c && c !== c.toUpperCase();
}

function hasLower(value: string): boolean {
    for (const c of value) {
        if (isLower(c)) {
            return true;
        }
    }
    return false;
}

function requiresLower(field: ValidationField) {
    return hasLower(field.value)
        ? ''
        : `${field.label} requires a lowercase letter`;
} 

export default requiresLower;
export { isLower, hasLower };