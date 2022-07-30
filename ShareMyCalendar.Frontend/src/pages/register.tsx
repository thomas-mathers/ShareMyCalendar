import { useCallback } from 'react';
import { Button, CircularProgress } from '@mui/material';
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
import useFetch from '../hooks/use-fetch/use-fetch';
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
    const { loading, execute } = useFetch({
        url: 'https://localhost:7040/user',
        options: {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(values)
        },
    });
    const handleClick = useCallback(() => execute(), [execute]);
    const visibility = loading ? 'visible' : 'hidden';
    return (
        <StackPage title="Register">
            {controls}
            <CircularProgress style={{ visibility }} />
            <Button disabled={!isPristine} variant="contained" color="primary" onClick={handleClick}>Ok</Button>
        </StackPage>
    );
}

export default Register