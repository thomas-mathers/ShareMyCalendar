import required from "../validators/required";

it.each([
    [''],
])('reports error for not empty string', (value) => {
    // Arrange
    const field = {
        label: 'name',
        value: value
    };

    // Act
    const actual = required(field);

    // Assert
    expect(actual).toBeTruthy();
});

it.each([
    ['a'],
    ['1'],
    ['!'],
    ['abc123']
])('does not report error for non empty string', (value) => {
    // Arrange
    const field = {
        label: 'name',
        value: value
    };

    // Act
    const actual = required(field);

    // Assert
    expect(actual).toBe('');
})