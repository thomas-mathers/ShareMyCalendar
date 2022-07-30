import { Field, FieldType, isValid } from "../fields";
import requiresDigit from "../validators/requires-digit";
import requiresLower from "../validators/requires-lower";
import requiresMinLength from "../validators/requires-min-length";
import requiresNonAlpha from "../validators/requires-non-alpha";
import requiresUpper from "../validators/requires-upper";

it.each([
    ['Freedom$8', true],
    ['Unicorn$8', true],
    ['abc', false],
    ['aBc123', false],
    ['a$', false]
])('returns expected value', (value: string, expected: boolean) => {
    // Arrange
    const field: Field = {
        type: FieldType.Text,
        label: 'field',
        value: value,
        validators: [
            requiresDigit,
            requiresLower,
            requiresMinLength(6),
            requiresNonAlpha,
            requiresUpper
        ],
        errorMessages: []
    };

    // Act
    const actual = isValid(field);

    // Assert
    expect(actual).toBe(expected);
});
