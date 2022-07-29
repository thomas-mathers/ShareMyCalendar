import { isLower } from "./requires-lower";
import { isUpper } from "./requires-upper";
import { isDigit } from "./requires-digit";
import ValidationField from "./validation-field";

function isNonAlpha(c: string): boolean {
    return !isUpper(c) && !isLower(c) && !isDigit(c)
}

function hasNonAlpha(value: string): boolean {
    for (const c of value) {
        if (isNonAlpha(c)) {
            return true;
        }
    }
    return false;
}

function requiresNonAlpha(field: ValidationField) {
    return hasNonAlpha(field.value)
        ? ''
        : `${field.label} requires a non alphanumeric character`;
}

export default requiresNonAlpha;
export { isNonAlpha, hasNonAlpha, }