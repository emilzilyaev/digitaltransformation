import React, {useEffect, useState} from "react";
import axios from 'axios';
import {useForm, Controller, FieldValues, UnpackNestedValue} from "react-hook-form";
import Select from 'react-select'

import "./styles.css";
import Recommendations from "../recommendations/Recommendations";

const MAX_PARAMETERS = 5;
const SERVER_URL = process.env.REACT_APP_SERVER_URL || 'http://62.84.123.217';

const PARAMS_URL = `${SERVER_URL}/Parameters`;
const USER_CHOICE = `${SERVER_URL}/Recommendation`;

type HandleInputProps = {
    value: any;
    onChange: (...event: any[]) => void;
    name: string;
    isMulti: boolean;
};

const makeOptions = (words: { [index: string]: string; }) =>
    Object.entries(words).map(([, value]) => {
        return {
            label: value,
            value: value
        }
    });

const Basic = () => {
    const {handleSubmit, control, getValues, setError, clearErrors, formState: {errors}} = useForm();
    const [modelParams, setParams] = useState<ParameterDefinition[] | undefined>([]);
    const [recommendations, setRecommendations] = useState<RecommendationInfo[] | undefined>([]);

    const onSubmit = () => {
        const data: UnpackNestedValue<FieldValues> = getValues();
        const parameters = [];
        for (let key in data) {
            if (!data.hasOwnProperty(key)) continue;
            const contents = data[key];
            const values = Array.isArray(contents) ? contents.map((it) => it.label) : [contents.label];
            parameters.push({id: key, values});
        }
        sendUserChoice({parameters});
    };

    const sendUserChoice = (values: GetRecommendation) => {
        axios.post<GetRecommendationResponse>(USER_CHOICE, {
            ...values
        }).then(({data}) => {
            setRecommendations(data.recommendations);
        })
    };

    const fetchParams = () => {
        axios.get<GetParametersResponse>(PARAMS_URL).then(({data}) => {
            const parameters = data.parameters || [];
            setParams(parameters);
        })
    };

    useEffect(() => {
        fetchParams();
    }, []);

    const handleChange = (props: HandleInputProps) => {
        const {value, onChange, name, isMulti} = props;
        if (isMulti && value.length > MAX_PARAMETERS) {
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
                {(modelParams || []).map((param: ParameterDefinition, index: number) => {
                    const isMulti = param.type === "MultiAcceptable";
                    return (
                        <div key={param.id}>
                            <label htmlFor={param.id}>{param.description}</label>
                            <Controller
                                render={
                                    ({field}) => <Select
                                        {...field}
                                        placeholder={param.description}
                                        onChange={(value) => handleChange({
                                            value,
                                            onChange: field.onChange,
                                            name: field.name,
                                            isMulti
                                        })}
                                        isMulti={isMulti}
                                        options={makeOptions(param.acceptableValues)}
                                    />
                                }
                                control={control}
                                name={param.id}
                                defaultValue={[]}
                            />
                            {errors[param.id] && <p>{errors[param.id].message}</p>}
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
