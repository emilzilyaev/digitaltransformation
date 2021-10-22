import React from "react";
import {useForm, Controller} from "react-hook-form";
import Select from 'react-select'

import "./styles.css";

const Basic = () => {
    const {handleSubmit, control, getValues} = useForm();
    const onSubmit = () => {
        alert(JSON.stringify(getValues()));
    };

    const options = [
        {value: 'ar', label: 'AR/VR'},
        {value: 'neurotechnology', label: 'Нейротехнологии'},
        {value: 'biotechnology', label: 'Биотехнологии'},
        {value: '3d', label: '3D моделирование'},
        {value: 'drones', label: 'Беспилотники'}
    ];

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
                                options={options}
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
                                isMulti
                                options={options}
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
                                isMulti
                                options={options}
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
                                options={options}
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
                                isMulti
                                options={options}
                            />
                        }
                        control={control}
                        name="department"
                        defaultValue={[]}
                    />
                </div>
                <input type="submit" value="Отправить данные"/>
            </form>
        </div>
    );
};

export default Basic;
