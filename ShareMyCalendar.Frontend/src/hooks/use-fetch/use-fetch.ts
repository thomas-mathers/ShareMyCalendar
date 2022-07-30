import { useCallback, useState } from "react";
import ApiResult from "../../models/api-result";

interface Props {
    url: string;
    options?: RequestInit;
    executeAutomatically?: boolean;
}

function useFetch<T>(props: Props) {
    const { url, options, executeAutomatically } = props;
    const [loading, setLoading] = useState<boolean>();
    const [errorCode, setErrorCode] = useState<string>();
    const [errorMessage, setErrorMessage] = useState<string>();
    const [value, setValue] = useState<T>();
    const execute = useCallback(async () => {
        try {
            setLoading(true);
            const response = await fetch(url, options);
            const json = await response.json();
            const responseBody: ApiResult<T> = json;
            setErrorCode(responseBody.errorCode);
            setErrorMessage(responseBody.errorMessage);
            setValue(responseBody.value);
        } catch (e) {
            setErrorMessage('An error has occured contacting an external service. Please try again');
            throw e;
        } finally {
            setLoading(false);
        }
    }, [url, options]);        
    if (executeAutomatically) {
        execute();
    }
    return { loading, errorCode, errorMessage, value, execute };
}

export default useFetch;