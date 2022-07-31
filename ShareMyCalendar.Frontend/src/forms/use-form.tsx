import { useCallback, useMemo, useReducer } from 'react';
import { TextField } from '@mui/material';
import ErrorMessage from '../components/error-message';
import { ActionType } from './actions';
import { areConstraintsSatisfied } from './binary-constraint';
import { FieldType, isValid } from './fields';
import initializeStateFromProps from './initialize-state-from-props';
import Props from './props';
import reducer from './reducer';

function useForm(props: Props) {
    const [state, dispatch] = useReducer(reducer, props, initializeStateFromProps);
    const { fields, constraints } = state;

    const values = useMemo(() => {
        return Object.fromEntries(Object.entries(fields).map(([k, v]) => [k, v.value]));
    }, [fields]);

    const isPristine = useMemo(() => {
        for (const field of Object.values(fields)) {
            if (!isValid(field)) {
                return false;
            }
        }
        return areConstraintsSatisfied(fields, constraints);
    }, [fields, constraints]);

    const handleFormFieldChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
        dispatch({
            type: ActionType.FieldChanged,
            payload: { name: e.target.name, value: e.target.value }
        });
    }, []);

    const handleFormFieldBlur = useCallback((e: React.FocusEvent<HTMLInputElement>) => {
        dispatch({
            type: ActionType.FieldChanged,
            payload: { name: e.target.name, value: fields[e.target.name].value }
        });
    }, [fields]);

    const controls = Object.entries(fields).map(([name, field]) => {
        switch (field.type) {
            case FieldType.Text:
                return (
                    <TextField
                        {...field.textFieldProps}
                        key={name}
                        name={name}
                        label={field.label}
                        value={field.value}
                        onChange={handleFormFieldChange}
                        onBlur={handleFormFieldBlur}
                        error={field.errorMessages.length > 0}
                        helperText={field.errorMessages.map((msg, i) => <ErrorMessage key={i} text={msg}/>)}
                    />
                ) 
            default:
                return undefined;
        }
    })

    return {
        isPristine,
        values,
        controls,
    }
}

export default useForm;