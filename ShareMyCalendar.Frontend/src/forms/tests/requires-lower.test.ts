import requiresLower from "../validators/requires-lower";

it.each([
    [''],
    ['123'],
    ['!@#'],
    ['ABC']
])('reports error if there is no lowercase letter', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresLower(field);

    // Assert
    expect(actual).toBeTruthy();
});

it.each([
    ['a'],
    ['123a'],
    ['ABcD'],
    ['a!@#'],
    ['ABCd123!@#']
])('does not report error if there is a lowercase letter', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresLower(field);

    // Assert
    expect(actual).toBe('');
})