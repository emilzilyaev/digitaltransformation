import React from "react";
import "./styles.css";

const formatNum = (num?: number) => {
    return ((num || 0) * 100).toFixed(1);
};

const RecommendationItem = (props: {item: RecommendationInfo, index: number}) => {
    const {item, index} = props;
    const width = (100 - index * 10) > 10 ? (100 - index * 10) : 10;
    return (
        <div className="recommendation-item">
            <div>
                <span className="recommendation-item__score">{formatNum(item.matchPercentage)}%</span>
                <span className="recommendation-item__title">{item.description}</span>
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
            <div className="spacer"/>
            <h1>Вам больше всего подойдет</h1>
            {
                // Показываем первые 30 результатов
                items.slice(0, 30).map((item, index) => <RecommendationItem index={index} item={item} key={item.id} />)
            }
        </div>
    )
};

export default List;
