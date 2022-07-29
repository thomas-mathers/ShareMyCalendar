import ValidationField from "./validation-field";

const regex = new RegExp(/^\S+@\S+\.\S+$/);

function isEmail(value: string) {
    return regex.test(value);
}

function email(field: ValidationField) {
    return isEmail(field.value)
        ? ''
        : `${field.label} must be an email`;
} 

export default email;
export { isEmail };