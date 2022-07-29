import email from "../validators/email";

it.each([
    [''],
    ['abc123'],
    ['a@'],
    ['@a'],
    ['a@a']
])('reports error for illegal emails', (value) => {
    // Arrange
    const field = {
        label: 'email',
        value: value
    };

    // Act
    const actual = email(field);

    // Assert
    expect(actual).toBeTruthy();
})

it.each([
    ['some.cool.guy@gmail.com'],
    ['some_guy@hotmail.com'],
    ['guy@gmail.com']
])('does not report error for actual emails', (value) => {
    // Arrange
    const field = {
        label: 'email',
        value: value
    };

    // Act
    const actual = email(field);

    // Assert
    expect(actual).toBe('');
})