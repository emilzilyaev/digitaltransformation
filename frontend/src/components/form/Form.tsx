import React, {useEffect, useState} from "react";
import axios from 'axios';
import {useForm, Controller, FieldValues, UnpackNestedValue} from "react-hook-form";
import Select from 'react-select'

import "./styles.css";
import Recommendations from "../recommendations/Recommendations";

const MAX_PARAMETERS = 5;
const SERVER_URL = process.env.REACT_APP_SERVER_URL;

const PARAMS_URL = `${SERVER_URL}/params`;
const USER_CHOICE = `${SERVER_URL}/save`;

type HandleInputProps = {
    value: any;
    onChange: (...event: any[]) => void;
    name: string;
};

const initialParams = {
    'department': [],
    'stage': [],
    'market': [],
    'technologies': [],
    'legal': []
};

const Basic = () => {
    const {handleSubmit, control, getValues, setError, clearErrors, formState: {errors}} = useForm();
    const [modelParams, setParams] = useState(initialParams);
    const [recommendations, setRecommendations] = useState([]);

    const onSubmit = () => {
        sendUserChoice(getValues());
        fetchParams();
    };

    const sendUserChoice = (values: UnpackNestedValue<FieldValues>) => {
        axios.post(USER_CHOICE, {
            body: values,
        }).then(res => {
            const {data}: any = res;
            setRecommendations(data.recommendations);
        })
    };

    const fetchParams = () => {
        axios.get(PARAMS_URL).then(res => {
            const {data}: any = res;
            setParams(data.params);
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
                <div>
                    <label htmlFor="legal">Юр.лицо</label>
                    <Controller
                        render={
                            ({field}) => <Select
                                {...field}
                                placeholder="Юр. лицо"
                                options={modelParams.legal}
                            />
                        }
                        control={control}
                        name="legal"
                        defaultValue={""}
                    />
                </div>
                <div>
                    <label htmlFor="technologies">Технологии</label>
                    <Controller
                        render={
                            ({field}) => <Select
                                {...field}
                                placeholder="Ваши технологии"
                                onChange={(value) => handleChange({
                                    value, onChange: field.onChange, name: field.name
                                })}
                                isMulti
                                options={modelParams.technologies}
                            />
                        }
                        control={control}
                        name="technologies"
                        defaultValue={[]}
                    />
                </div>
                <div>
                    <label htmlFor="market">Рынок</label>
                    <Controller
                        render={
                            ({field}) => <Select
                                {...field}
                                placeholder="Рынок"
                                onChange={(value) => handleChange({
                                    value, onChange: field.onChange, name: field.name
                                })}
                                isMulti
                                options={modelParams.market}
                            />
                        }
                        control={control}
                        name="market"
                        defaultValue={[]}
                    />
                </div>
                <div>
                    <label htmlFor="stage">Стадия развития</label>
                    <Controller
                        render={
                            ({field}) => <Select
                                {...field}
                                placeholder="Стадия развития"
                                options={modelParams.stage}
                            />
                        }
                        control={control}
                        name="stage"
                        defaultValue={""}
                    />
                </div>
                <div>
                    <label htmlFor="department">Технологическая ниша</label>
                    <Controller
                        render={
                            ({field}) => <Select
                                {...field}
                                placeholder="Технологическая ниша"
                                onChange={(value) => handleChange({
                                    value, onChange: field.onChange, name: field.name
                                })}
                                isMulti
                                options={modelParams.department}
                            />
                        }
                        control={control}
                        name="department"
                        defaultValue={[]}
                    />
                    {errors.department && <p>{errors.department.message}</p>}
                </div>
                <input type="submit" value="Отправить данные"/>
            </form>
            <Recommendations items={recommendations}/>
        </div>
    );
};

export default Basic;
