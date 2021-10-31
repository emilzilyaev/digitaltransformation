/* Options:
Date: 2021-10-31 18:40:36
Version: 5.121
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: http://62.84.123.217

//GlobalNamespace:
//MakePropertiesOptional: True
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion:
//AddDescriptionAsComments: True
//IncludeTypes:
//ExcludeTypes:
//DefaultImports:
*/


interface IReturn<T>
{
}

interface IReturnVoid
{
}

/**
 * Значение параметра
 */
interface ParameterValue
{
    /**
     * Идентификатор параметра
     */
    id?: string;
    /**
     * Список значений
     */
    values?: string[];
}

type ParameterType = "NumberRange" | "OneAcceptable" | "MultiAcceptable";

/**
 * Список параметров
 */
interface ParameterDefinition
{
    /**
     * Идентификатор параметра
     */
    id: string;
    /**
     * Описание параметра
     */
    description: string;
    /**
     * Тип параметра
     */
    type?: ParameterType;
    /**
     * Список допустимых значений
     */
    acceptableValues: { [index: string]: string; };
}

/**
 * Рекомендация
 */
interface RecommendationInfo
{
    /**
     * Идентификатор рекомендации
     */
    id?: string;
    /**
     * Название
     */
    description?: string;
    /**
     * Процент совпадения
     */
    matchPercentage?: number;
}

interface ParametersCombination
{
    /**
     * Список параметров
     */
    parameters?: ParameterValue[];
    /**
     * Дата создания
     */
    created?: string;
}

/**
 * Результат получения описания параметров
 */
interface GetParametersResponse
{
    /**
     * Список параметров
     */
    parameters?: ParameterDefinition[];
}

/**
 * Результат получения рекомендаций по параметрам
 */
interface GetRecommendationResponse
{
    /**
     * Список рекомендаций
     */
    recommendations?: RecommendationInfo[];
}

/**
 * Результат получения истории комбинаций параметров
 */
interface GetParametersHistoryResponse
{
    /**
     * Список комбинаций параметров
     */
    combinations?: ParametersCombination[];
}

// @Route("/Parameters", "GET")
interface GetParameters extends IReturn<GetParametersResponse>
{
}

// @Route("/Recommendation", "POST")
interface GetRecommendation extends IReturn<GetRecommendationResponse>
{
    /**
     * Список параметров
     */
    // @ApiMember(Description="Список параметров")
    parameters?: ParameterValue[];
}

// @Route("/Recommendation/{RecommendationId}", "PUT")
interface UpdateRecommendation
{
    /**
     * Список параметров
     */
    // @ApiMember(Description="Список параметров", IsRequired=true)
    parameters: ParameterValue[];

    /**
     * Идентификатор рекомендации
     */
    recommendationId?: string;
}

// @Route("/Parameters/History", "GET")
interface GetParametersHistory extends IReturn<GetParametersHistoryResponse>
{
}
