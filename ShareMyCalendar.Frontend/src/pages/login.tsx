import { useMemo, useCallback } from 'react';
import { useNavigate, Link as RouterLink, } from 'react-router-dom';
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
import { FieldType, required, useForm, useFetch } from 'thomasmathers.react.hooks';
import ErrorList from '../components/error-list';
import { ApiResult, ApiValidationError, LoginSuccessResponse } from '../responses';
import StackPage from './stack-page';
import {
  LoginSocialFacebook,
  LoginSocialGoogle,
  LoginSocialTwitter,
  LoginSocialApple,
  IResolveParams,
} from 'reactjs-social-login';
import {
  AppleLoginButton,
  FacebookLoginButton,
  GoogleLoginButton,
  TwitterLoginButton
} from "react-social-login-buttons";

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
        <LoginSocialTwitter
          client_id={process.env.REACT_APP_TWITTER_CLIENT_ID || ''}
          redirect_uri={process.env.REACT_APP_BASE_URL || ''}
          onLoginStart={handleLoginStart}
          onResolve={handleLoginResolve}
          onReject={handleLoginReject}
          isOnlyGetToken={true}
        >
          <TwitterLoginButton style={{ fontSize: '1rem' }} />
        </LoginSocialTwitter>
        <LoginSocialApple
          client_id={process.env.REACT_APP_APPLE_CLIENT_ID || ''}
          redirect_uri={process.env.REACT_APP_BASE_URL || ''}
          onLoginStart={handleLoginStart}
          onResolve={handleLoginResolve}
          onReject={handleLoginReject}
        >
          <AppleLoginButton style={{ fontSize: '1rem' }} />
        </LoginSocialApple>
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