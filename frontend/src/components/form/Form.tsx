import React, {useEffect, useState} from "react";
import axios from 'axios';
import {useForm, Controller, FieldValues, UnpackNestedValue} from "react-hook-form";
import Select from 'react-select'

import "./styles.css";
import Recommendations from "../recommendations/Recommendations";

const MAX_PARAMETERS = 5;
const SERVER_URL = process.env.REACT_APP_SERVER_URL;

const PARAMS_URL = `${SERVER_URL}/Parameters`;
const USER_CHOICE = `${SERVER_URL}/Recommendation`;

type HandleInputProps = {
    value: any;
    onChange: (...event: any[]) => void;
    name: string;
};

const Basic = () => {
    const {handleSubmit, control, getValues, setError, clearErrors, formState: {errors}} = useForm();
    const [modelParams, setParams] = useState<ParameterDefinition[] | undefined>([]);
    const [recommendations, setRecommendations] = useState<RecommendationInfo[] | undefined>([]);

    const onSubmit = () => {
        sendUserChoice(getValues());
        fetchParams();
    };

    const sendUserChoice = (values: UnpackNestedValue<FieldValues>) => {
        axios.post<GetRecommendationResponse>(USER_CHOICE, {
            body: values,
        }).then(({data}) => {
            setRecommendations(data.recommendations);
        })
    };

    const fetchParams = () => {
        axios.get<GetParametersResponse>(PARAMS_URL).then(({data}) => {
            setParams(data.parameters);
        })
    };

    useEffect(() => {
        fetchParams();
    }, []);

    const handleChange = (props: HandleInputProps) => {
        const {value, onChange, name} = props;
        if (value.length > MAX_PARAMETERS) {
            setError(name, {
                type: "manual",
                message: "Можно выбрать не более 5 параметров",
            });
        } else {
            clearErrors(name);
            onChange(value);
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                {(modelParams || []).map((param: ParameterDefinition) => {
                    return (
                        <div>
                            <label htmlFor="legal">{param.id}</label>
                            <Controller
                                render={
                                    ({field}) => <Select
                                        {...field}
                                        placeholder={param.id}
                                        isMulti={param.type === "MultiAcceptable"}
                                        options={param.acceptableValues}
                                    />
                                }
                                control={control}
                                name={param.id ?? ""}
                                defaultValue={param.type === "MultiAcceptable" ? [] : ""}
                            />
                            {errors[param.id ?? ""] && <p>{errors[param.id ?? ""].message}</p>}
                        </div>
                    )
                })}
                <input type="submit" value="Отправить данные"/>
            </form>
            <Recommendations items={recommendations}/>
        </div>
    );
};

export default Basic;
