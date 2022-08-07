import { useMemo, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box,
    Button,
    Checkbox,
    Divider,
    FormControlLabel,
    FormGroup,
    LinearProgress,
    Link,
    Stack
} from '@mui/material';
import ErrorList from '../components/error-list';
import { FieldType, required, useForm } from '../forms';
import useFetch from '../hooks/use-fetch';
import { ApiValidationError, LoginSuccessResponse } from '../responses';
import StackPage from './stack-page';
import {
    LoginSocialFacebook,
    LoginSocialInstagram,
    LoginSocialTwitter,
    LoginSocialGoogle,
    LoginSocialMicrosoft,
    IResolveParams,
} from 'reactjs-social-login'

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
    }, [execute, navigate]);

    const onLoginStart = useCallback(() => {
    }, [])

    return (
        <StackPage title="Login">
            <LoginSocialFacebook
                appId={process.env.REACT_APP_FB_APP_ID || ''}
                onLoginStart={onLoginStart}
                onResolve={(resolveParams: IResolveParams) => {
                    console.log(resolveParams);
                }}
                onReject={(err) => {
                    console.log(err)
                }}
            >
                <Button>Facebook Login</Button>
            </LoginSocialFacebook>
            <Divider>OR</Divider>
            {controls}
            <Stack direction="row" alignItems="center" justifyContent="space-between">
                <FormGroup>
                    <FormControlLabel control={<Checkbox defaultChecked />} label="Remember me?" />
                </FormGroup>
                <Link href="#">Forgot password?</Link>
            </Stack>
            {fetching && <LinearProgress />}
            {errors.length > 0 && <ErrorList errors={errors} />}
            <Button variant="contained" color="primary" disabled={!isPristine} onClick={handleClick}>Login</Button>
            <Box textAlign="center">
                Need an account? <Link href="register">Sign Up</Link>
            </Box>
        </StackPage>
    );
}

export default Login