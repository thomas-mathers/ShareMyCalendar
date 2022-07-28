import { Button } from '@mui/material';
import useForm from '../hooks/useForm';
import StackPage from './stack-page';

function Register() {
    const { isPristine, values, fields } = useForm({
        username: {
            label: 'Username',
            value: '',
            validators: [
                (x) => x ? '' : 'Username is required'
            ]
        },
        email: {
            label: 'Email',
            value: '',
            validators: [
                (x) => x ? '' : 'Username is required'
            ]
        },
        password: {
            label: 'Password',
            value: '',
            validators: [
                (x) => x ? '' : 'Username is required'
            ]
        },
        confirmPassword: {
            label: 'Confirm Password',
            value: '',
            validators: [
                (x) => x !== values.password ? 'Must match password' : ''
            ]
        }
    });
    console.log(values);

    return (
        <StackPage title="Register">
            {fields}
            <Button disabled={!isPristine} variant="contained" color="primary">Ok</Button>
        </StackPage>
    );
}

export default Register