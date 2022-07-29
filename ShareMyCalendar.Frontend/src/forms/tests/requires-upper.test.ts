import requiresUpper from "../validators/requires-upper";

it.each([
    [''],
    ['123'],
    ['!@#'],
    ['abc']
])('reports error if there is no uppercase letter', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresUpper(field);

    // Assert
    expect(actual).toBeTruthy();
});

it.each([
    ['A'],
    ['123A'],
    ['abCd'],
    ['A!@#'],
    ['abcD123!@#']
])('does not report error if there is a uppercase letter', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresUpper(field);

    // Assert
    expect(actual).toBe('');
})