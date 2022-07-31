import { useCallback, useMemo } from 'react';
import { useNavigate } from "react-router-dom";
import { Box, Button, LinearProgress } from '@mui/material';
import ErrorMessage from '../components/error-message';
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
import { ApiValidationError, RegisterSuccessResponse } from '../responses';
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

    const { fetching, response, execute } = useFetch<ApiValidationError[], RegisterSuccessResponse>({
        url: 'https://localhost:7040/user',
        options: {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(values)
        },
    });

    const navigate = useNavigate();

    const errors = useMemo(() => {
        if (response?.error) {
            return response.error.map(e => e.description);
        }
        return [];
    }, [response?.error]);

    const handleClick = useCallback(async () => {
        const response = await execute();
        if (response.value?.id) {
            navigate('/');
        }
    }, [execute]);

    return (
        <StackPage title="Register">
            {controls}
            {fetching && <LinearProgress />}
            <Box>
                {errors.length > 0 && errors.map((e, i) => <ErrorMessage key={i} text={e}/>)}
            </Box>
            <Button disabled={!isPristine} variant="contained" color="primary" onClick={handleClick}>Ok</Button>
        </StackPage>
    );
}

export default Register