import { FieldType } from "../fields"
import initializeStateFromProps from "../initialize-state-from-props"
import Props from "../props"
import email from "../validators/email"
import required from "../validators/required"
import requiresDigit from "../validators/requires-digit"
import requiresLower from "../validators/requires-lower"
import requiresMinLength from "../validators/requires-min-length"
import requiresNonAlpha from "../validators/requires-non-alpha"
import requiresUpper from "../validators/requires-upper"

test('initializes correctly', () => {
    // Arrange
    var props: Props = {
        fields: {
            username: {
                type: FieldType.Text,
                label: 'Username',
                validators: [
                    required
                ],
                textFieldProps: {
                    required: true
                }
            },
            email: {
                type: FieldType.Text,
                label: 'Email',
                validators: [
                    required,
                    email
                ],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'email'
                    },
                }
            },
            password: {
                type: FieldType.Text,
                label: 'Password',
                validators: [
                    requiresMinLength(6),
                    requiresLower,
                    requiresUpper,
                    requiresDigit,
                    requiresNonAlpha,
                ],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'password'
                    },
                },
            },
            confirmPassword: {
                type: FieldType.Text,
                label: 'Confirm Password',
                validators: [
                    required,
                ],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'password'
                    },
                }
            },
        },
        constraints: [
            { op: '==', lparam: 'password', rparam: 'confirmPassword' }
        ]
    }

    // Act
    const actual = initializeStateFromProps(props);

    // Assert
    expect(actual).toMatchObject({
        fields: {
            username: {
                type: FieldType.Text,
                label: 'Username',
                value: '',
                errorMessages: [],
                textFieldProps: {
                    required: true
                }
            },
            email: {
                type: FieldType.Text,
                label: 'Email',
                value: '',
                errorMessages: [],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'email'
                    },
                }
            },
            password: {
                type: FieldType.Text,
                label: 'Password',
                value: '',
                errorMessages: [],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'password'
                    },
                },
            },
            confirmPassword: {
                type: FieldType.Text,
                label: 'Confirm Password',
                value: '',
                errorMessages: [],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'password'
                    },
                }
            },
        },
        constraints: [
            { op: '==', lparam: 'password', rparam: 'confirmPassword' }
        ]
    });
    expect(actual.fields.username.validators.length).toBe(props.fields.username.validators?.length);
    expect(actual.fields.email.validators.length).toBe(props.fields.email.validators?.length);
    expect(actual.fields.password.validators.length).toBe(props.fields.password.validators?.length);
    expect(actual.fields.confirmPassword.validators.length).toBe(props.fields.confirmPassword.validators?.length);
})