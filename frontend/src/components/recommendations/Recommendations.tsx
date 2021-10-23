import React from "react";
import "./styles.css";

export interface Recommendation {
    id: string;
    title: string;
    score: number;
}

const RecommendationItem = (props: {item: Recommendation, index: number}) => {
    const {item, index} = props;
    const width = (100 - index * 10) > 10 ? (100 - index * 10) : 10;
    return (
        <div className="recommendation-item">
            <div>
                <span className="recommendation-item__score">{item.score}%</span>
                <span className="recommendation-item__title">{item.title}</span>
            </div>
            <div style={{
                width: `${width}%`
            }} className="recommendation-item__progress"/>
        </div>
    )
};

export type RecommendationProps = {
    items: Recommendation[];
}

const List = (props: RecommendationProps) => {
    const {items} = props;

    if (!items.length) {
        return null;
    }
    return (
        <div>
            <h1>Вам больше всего подойдет</h1>
            {
                items.map((item, index) => <RecommendationItem index={index} item={item} key={item.id} />)
            }

        </div>
    )
};

export default List;
