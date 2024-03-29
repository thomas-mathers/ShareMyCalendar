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
import { useCallback, useMemo } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import {
  FacebookLoginButton,
  GoogleLoginButton
} from "react-social-login-buttons";
import {
  IResolveParams,
  LoginSocialFacebook,
  LoginSocialGoogle
} from 'reactjs-social-login';
import { FieldType, required, useFetch, useForm } from 'thomasmathers.react.hooks';
import ErrorList from '../components/error-list';
import { ApiResult, ApiValidationError, LoginSuccessResponse } from '../responses';
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

  const { fetching, response, execute } = useFetch<ApiResult<ApiValidationError[], LoginSuccessResponse>>({
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

  const handleLoginStart = useCallback(() => {

  }, []);

  const handleLoginResolve = useCallback((resolveParams: IResolveParams) => {
    console.log(resolveParams);
  }, []);

  const handleLoginReject = useCallback(() => {

  }, []);

  return (
    <StackPage title="Login">
      <Stack>
        <LoginSocialFacebook
          appId={process.env.REACT_APP_FB_APP_ID || ''}
          onLoginStart={handleLoginStart}
          onResolve={handleLoginResolve}
          onReject={handleLoginReject}
          isOnlyGetToken={true}
        >
          <FacebookLoginButton style={{ fontSize: '1rem' }} />
        </LoginSocialFacebook>
        <LoginSocialGoogle
          client_id={process.env.REACT_APP_GOOGLE_CLIENT_ID || ''}
          onLoginStart={handleLoginStart}
          onResolve={handleLoginResolve}
          onReject={handleLoginReject}
          isOnlyGetToken={true}
        >
          <GoogleLoginButton style={{ fontSize: '1rem' }} />
        </LoginSocialGoogle>
      </Stack>
      <Divider>OR</Divider>
      {controls}
      <Box display="flex" flexDirection="row" alignItems="center" justifyContent="space-between">
        <FormGroup>
          <FormControlLabel control={<Checkbox defaultChecked />} label="Remember me?" />
        </FormGroup>
        <Link component={RouterLink} to="/forgot-password">Forgot password?</Link>
      </Box>
      {fetching && <LinearProgress />}
      {errors.length > 0 && <ErrorList errors={errors} />}
      <Button variant="contained" color="primary" disabled={!isPristine} onClick={handleClick}>Login</Button>
      <Box textAlign="center">
        Need an account? <Link component={RouterLink} to="/register">Sign Up</Link>
      </Box>
    </StackPage>
  );
}

export default Login