/* Options:
Date: 2021-10-23 15:01:43
Version: 5.121
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://localhost:5001

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


interface IReturn<T> {
}

interface IReturnVoid {
}

/**
* �������� ���������
*/
interface ParameterValue {
    /**
    * ������������� ���������
    */
    id?: string;
    /**
    * ������ ��������
    */
    values?: string[];
}

type ParameterType = "NumberRange" | "OneAcceptable" | "MultiAcceptable";

/**
* ������ ����������
*/
interface ParameterDefinition {
    /**
    * ������������� ���������
    */
    id: string;
    /**
    * ��� ���������
    */
    type?: ParameterType;
    /**
    * ������ ���������� ��������
    */
    acceptableValues?: string[];
}

/**
* ������������
*/
interface RecommendationInfo {
    /**
    * ������������� ������������
    */
    id?: string;
    /**
    * ��������
    */
    description?: string;
    /**
    * ������� ����������
    */
    matchPercentage?: number;
}

interface ParametersCombination {
    /**
    * ������ ����������
    */
    parameters?: ParameterValue[];
    /**
    * ���� ��������
    */
    created?: string;
}

/**
* ��������� ��������� �������� ����������
*/
interface GetParametersResponse {
    /**
    * ������ ����������
    */
    parameters?: ParameterDefinition[];
}

/**
* ��������� ��������� ������������ �� ����������
*/
interface GetRecommendationResponse {
    /**
    * ������ ������������
    */
    recommendations?: RecommendationInfo[];
}

/**
* ��������� ��������� ������� ���������� ����������
*/
interface GetParametersHistoryResponse {
    /**
    * ������ ���������� ����������
    */
    combinations?: ParametersCombination[];
}

// @Route("/Parameters", "GET")
interface GetParameters extends IReturn<GetParametersResponse> {
}

// @Route("/Recommendation", "POST")
interface GetRecommendation extends IReturn<GetRecommendationResponse> {
    /**
    * ������ ����������
    */
    // @ApiMember(Description="������ ����������", IsRequired=true)
    parameters: ParameterValue[];
}

// @Route("/Recommendation/{RecommendationId}", "PUT")
interface UpdateRecommendation {
    /**
    * ������ ����������
    */
    // @ApiMember(Description="������ ����������", IsRequired=true)
    parameters: ParameterValue[];

    /**
    * ������������� ������������
    */
    recommendationId?: string;
}

// @Route("/Parameters/History", "GET")
interface GetParametersHistory extends IReturn<GetParametersHistoryResponse> {
}
