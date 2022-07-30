import { Field, FieldType, validate } from "../fields";
import requiresDigit from "../validators/requires-digit";
import requiresLower from "../validators/requires-lower";
import requiresMinLength from "../validators/requires-min-length";
import requiresNonAlpha from "../validators/requires-non-alpha";
import requiresUpper from "../validators/requires-upper";

it.each([
    ['a'],
    ['ab'],
    ['abcd'],
    ['abcde']
])('returns correct validation messages with short input', (value: string) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'Password',
        value: value,
        validators: [
            requiresMinLength(6),
            requiresLower,
            requiresUpper,
            requiresDigit,
            requiresNonAlpha,
        ],
        errorMessages: []
    };

    // Act
    validate(field);

    // Assert
    expect(field.errorMessages).toContain('Password must be at least 6 characters long')
});

it.each([
    ['ABC'],
    ['123'],
    ['!@#'],
    ['ABC123'],
    ['ABC123!@#']
])('returns correct validation messages with no lowercase', (value: string) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'Password',
        value: value,
        validators: [
            requiresMinLength(6),
            requiresLower,
            requiresUpper,
            requiresDigit,
            requiresNonAlpha,
        ],
        errorMessages: []
    };

    // Act
    validate(field);

    // Assert
    expect(field.errorMessages).toContain('Password requires a lowercase letter')
});

it.each([
    ['abc'],
    ['123'],
    ['!@#'],
    ['abc123'],
    ['abc123!@#']
])('returns correct validation messages with no uppercase', (value: string) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'Password',
        value: value,
        validators: [
            requiresMinLength(6),
            requiresLower,
            requiresUpper,
            requiresDigit,
            requiresNonAlpha,
        ],
        errorMessages: []
    };

    // Act
    validate(field);

    // Assert
    expect(field.errorMessages).toContain('Password requires an uppercase letter')
});

it.each([
    ['abc'],
    ['ABC'],
    ['!@#'],
    ['abcDEF'],
    ['abcDEF!@#']
])('returns correct validation messages with no digit', (value: string) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'Password',
        value: value,
        validators: [
            requiresMinLength(6),
            requiresLower,
            requiresUpper,
            requiresDigit,
            requiresNonAlpha,
        ],
        errorMessages: []
    };

    // Act
    validate(field);

    // Assert
    expect(field.errorMessages).toContain('Password requires a digit')
});

it.each([
    ['abc'],
    ['ABC'],
    ['123'],
    ['abcDEF'],
    ['abcDEF123']
])('returns correct validation messages with no non alphanumeric character', (value: string) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'Password',
        value: value,
        validators: [
            requiresMinLength(6),
            requiresLower,
            requiresUpper,
            requiresDigit,
            requiresNonAlpha,
        ],
        errorMessages: []
    };

    // Act
    validate(field);

    // Assert
    expect(field.errorMessages).toContain('Password requires a non alphanumeric character')
});

it.each([
    ['Freedom$8']
])('returns no validation messages with valid input', (value: string) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'Password',
        value: value,
        validators: [
            requiresMinLength(6),
            requiresLower,
            requiresUpper,
            requiresDigit,
            requiresNonAlpha,
        ],
        errorMessages: []
    };

    // Act
    validate(field);

    // Assert
    expect(field.errorMessages).toStrictEqual([])
});