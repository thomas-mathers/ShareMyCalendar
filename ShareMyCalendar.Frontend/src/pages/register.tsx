import { Button } from '@mui/material';
import {
    email,
    required,
    requiresDigit,
    requiresLower,
    requiresMinLength,
    requiresNonAlpha,
    requiresUpper,
    FieldType,
    useForm,
} from '../forms';
import StackPage from './stack-page';

function Register() {
    const { isPristine, values, controls } = useForm({
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
    });

    return (
        <StackPage title="Register">
            {controls}
            <Button disabled={!isPristine} variant="contained" color="primary">Ok</Button>
        </StackPage>
    );
}

export default Register