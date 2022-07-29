import requiresNonAlpha from "../validators/requires-non-alpha";

it.each([
    [''],
    ['123'],
    ['abc'],
    ['ABC'],
    ['abcDEF123']
])('reports error if there is no non-alpha letter', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresNonAlpha(field);

    // Assert
    expect(actual).toBeTruthy();
});

it.each([
    ['!'],
    ['123@'],
    ['AB#D'],
    ['!@#$'],
    ['ABCd123!@#']
])('does not report error if there is a non-alpha letter', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresNonAlpha(field);

    // Assert
    expect(actual).toBe('');
})