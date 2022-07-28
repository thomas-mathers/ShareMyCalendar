import { TextField } from '@mui/material';
import { useCallback, useMemo, useState } from 'react';

type Validator = (x: string) => string;

interface FormField {
    label: string;
    value: string;
    validators: Validator[];
    validationErrorMessage?: string;
}

type Form = Record<string, FormField>;

interface Props {
    initialForm: Form;
}

function useForm(initialForm: Form) {
    const [form, setForm] = useState<Form>(initialForm);

    const values = useMemo(() => {
        const values: Record<string, string> = {};
        for (const [k, v] of Object.entries(form)) {
            values[k] = v.value;
        };
        return values;
    }, [form]);

    const isPristine = useMemo(() => {
        for (let field of Object.values(form)) {
            for (let validator of field.validators) {
                if (!!validator(field.value)) {
                    return false;
                }
            }
        }
        return true;
    }, [form]);

    const handleFormFieldChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
        const name = e.target.name;
        const value = e.target.value;
        setForm({
            ...form,
            [name]: {
                ...form[name],
                value,
            }
        })
    }, [form]);

    const handleFormFieldBlur = useCallback((e: React.FocusEvent<HTMLInputElement>) => {
        const name = e.currentTarget.name;
        const value = form[name].value;
        const validationErrorMessage = form[name].validators.map(f => f(value)).join('\n');
        setForm({
            ...form,
            [name]: {
                ...form[name],
                validationErrorMessage, 
            }
        })
    }, [form]);

    const fields = Object.entries(form).map(([fieldName, field]) => (
        <TextField
            key={fieldName}
            name={fieldName}
            label={field.label}
            value={field.value}
            onChange={handleFormFieldChange}
            onBlur={handleFormFieldBlur}
            error={!!field.validationErrorMessage}
            helperText={field.validationErrorMessage}
        />
    ))

    return {
        isPristine,
        values,
        fields,
    }
}

export default useForm;