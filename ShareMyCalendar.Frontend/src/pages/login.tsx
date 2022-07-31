import { useMemo, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, LinearProgress } from '@mui/material';
import ErrorMessage from '../components/error-message';
import { FieldType, required, useForm } from '../forms';
import useFetch from '../hooks/use-fetch/use-fetch';
import { ApiValidationError, LoginSuccessResponse } from '../responses';
import StackPage from './stack-page';

function Login() {
    const { controls, values, isPristine } = useForm({
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
            password: {
                type: FieldType.Text,
                label: 'Password',
                validators: [
                    required
                ],
                textFieldProps: {
                    required: true,
                    InputProps: {
                        type: 'password'
                    }
                }
            }
        },
        constraints: []
    });

    const { fetching, response, execute } = useFetch<ApiValidationError[], LoginSuccessResponse>({
        url: 'https://localhost:7040/user/login',
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
        if (response.value) {
            navigate('/dashboard');
        }
    }, [execute]);

    return (
        <StackPage title="Login">
            {controls}
            {fetching && <LinearProgress />}
            <Box>
                {errors.length > 0 && errors.map((e, i) => <ErrorMessage key={i} text={e} />)}
            </Box>
            <Button variant="contained" color="primary" disabled={!isPristine} onClick={handleClick}>Ok</Button>
        </StackPage>
    );
}

export default Login