import React from "react";
import "./styles.css";

const RecommendationItem = (props: {item: RecommendationInfo, index: number}) => {
    const {item, index} = props;
    const width = (100 - index * 10) > 10 ? (100 - index * 10) : 10;
    return (
        <div className="recommendation-item">
            <div>
                <span className="recommendation-item__score">{item.matchPercentage}%</span>
                <span className="recommendation-item__title">{item.id}</span>
            </div>
            <div style={{
                width: `${width}%`
            }} className="recommendation-item__progress"/>
        </div>
    )
};

export type RecommendationProps = {
    items?: RecommendationInfo[];
}

const List = (props: RecommendationProps) => {
    const {items} = props;

    if (!items || !items.length) {
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
