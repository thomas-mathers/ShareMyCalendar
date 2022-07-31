import { useCallback, useState } from "react";
import ApiResult from "../../responses/api-result";

interface Props {
    url: string;
    options?: RequestInit;
    executeAutomatically?: boolean;
}

function useFetch<TError, TValue>(props: Props) {
    const { url, options, executeAutomatically } = props;
    const [fetching, setFetching] = useState<boolean>();
    const [response, setResponse] = useState<ApiResult<TError, TValue>>();
    const execute = useCallback(async () => {
        try {
            setFetching(true);
            setResponse(undefined);
            const response = await fetch(url, options);
            const json = await response.json();
            const responseBody: ApiResult<TError, TValue> = json;
            setResponse(responseBody);
            return responseBody;
        } finally {
            setFetching(false);
        }
    }, [url, options]);        
    if (executeAutomatically) {
        execute();
    }
    return { fetching, response, execute };
}

export default useFetch;