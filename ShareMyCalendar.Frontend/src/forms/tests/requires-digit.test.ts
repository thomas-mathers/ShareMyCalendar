import requiresDigit from "../validators/requires-digit";

it.each([
    [''],
    ['abc'],
    ['!@#']
])('reports error if there is no digit', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresDigit(field);

    // Assert
    expect(actual).toBeTruthy();
});

it.each([
    ['1abc'],
    ['ab1c'],
    ['abc1'],
    ['abc123']
])('does not report error if there is digit', (value) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresDigit(field);

    // Assert
    expect(actual).toBe('');
})