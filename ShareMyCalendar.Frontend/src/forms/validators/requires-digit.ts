import ValidationField from "./validation-field";

function isDigit(c: string): boolean {
    return c === '0'
        || c === '1'
        || c === '2'
        || c === '3'
        || c === '4'
        || c === '5'
        || c === '6'
        || c === '7'
        || c === '8'
        || c === '9';
}

function hasDigit(value: string): boolean {
    for (const c of value) {
        if (isDigit(c)) {
            return true;
        }
    }
    return false;
}

function requiresDigit(field: ValidationField) {
    return hasDigit(field.value)
        ? ''
        : `${field.label} requires a digit`;
} 

export default requiresDigit;
export { isDigit, hasDigit }