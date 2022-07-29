import requiresMinLength from "../validators/requires-min-length";

it.each([
    ['abc', 4],
    ['abc', 10],
    ['abc', 100],
])('reports error if value has length less than required', (value, length) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresMinLength(length)(field);

    // Assert
    expect(actual).toBeTruthy();
});

it.each([
    ['!', 1],
    ['123@', 4],
    ['AB#D', 4],
    ['!@#$', 4],
    ['ABCd123!@#', 10]
])('does not report error if value has length at least as large as required', (value, length) => {
    // Arrange
    const field = {
        label: 'password',
        value: value
    };

    // Act
    const actual = requiresMinLength(length)(field);

    // Assert
    expect(actual).toBe('');
})