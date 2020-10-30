# Aqualab
## Logging Events
Each log is sent with a number of fields required by [SimpleLog](https://github.com/fielddaylab/simplelog). Simple log allows for a custom field called event_data_complex along with its category enumerator:
  event_custom: category enumerator
  event_data_complex: JSON.stringify(log_data)
Each log_data is a JSON object for that specific category as defined below.

#### Bugs
1. When a behavior is changed, the previous sync is logged rather than the updated sync.

#### Version Log
Versions:
1. Alpha

### Event Categories
1. [modelingbehaviorchange](#modelingbehaviorchange)
2. [modelingtickchange](#modelingtickchange)
3. [modelingsyncchange](#modelingsyncchange)
4. [argumentationresponseclick](#argumentationresponseclick)
5. [argumentationtabclick](#argumentationtabclick)
6. [argumentationtypeclick](#argumentationtypeclick)


### Data Structures
1. [Rule Data](#RuleData)
2. [Model Data](#ModelData)

<a name="modelingbehaviorchange"/>

## Event Categories
#### modelingbehaviorchange (index=1)
| Key | Value | Description |
| --- | --- | --- |
| scenario_id | scenario_id | id of the current scenario being modeled |
| curr_tick | curr_tick | the current tick of the model being viewed |
| curr_sync | curr_sync | the current sync value between the model and the data |
| prev_value | prev_value | the previous value for this behavior |
| rule_data | rule_data | the organism affected by the change, the type of behavior being changed, and the new value for that behavior |

<a name="modelingtickchange"/>

#### modelingtickchange (index=2)
| Key | Value | Description |
| --- | --- | --- |
| scenario_id | scenario_id | id of the current scenario being modeled |
| click_location | "bar", "prev_button", "next_button" | the location on the tick slider that was clicked |
| prev_tick | prev_tick | the previous tick of the model being viewed |
| new_tick | new_tick | the new tick of the model being viewed |

<a name="modelingsyncchange"/>

#### modelingsyncchange (index=3)
| Key | Value | Description |
| --- | --- | --- |
| scenario_id | scenario_id | id of the current scenario being modeled |
| prev_sync | prev_sync | the previous sync value between the model and the data |
| new_sync | new_sync | the new sync value between the model and the data |

<a name="argumentationresponseclick"/>

#### argumentationresponseclick (index=4)
| Key | Value | Description |
| --- | --- | --- |
| argument_id | argument_id | id for the current argument |
| response_id | response_id | id for the response that was clicked |
| valid | bool | indicates if response was valid or invalid |

<a name="argumentationtabclick"/>

#### argumentationtabclick (index=5)
| Key | Value | Description |
| --- | --- | --- |
| argument_id | argument_id | id for the current argument |
| new_tab | new_tab | name of the tab that was clicked |

<a name="argumentationtypeclick"/>

#### argumentationtypeclick (index=6)
| Key | Value | Description |
| --- | --- | --- |
| argument_id | argument_id | id for the current argument |
| new_type | new_type | name of the type that was clicked |

## Data Structures
<a name="RuleData"/>

#### Rule Data
| Key | Value | Description |
| --- | --- | --- |
| organism | organism | the organism that this rule data describes |
| value_type | value_type | the type of behavior that this value describes |
| curr_value | curr_value | numeric value describing this behavior |

<a name="ModelData"/>

#### Model Data
| Key | Value | Description |
| --- | --- | --- |
| updated_rule_data | updated_rule_data | data about the changed rule which caused a change in sync |
| curr_tick | curr_tick | the current tick of the model being viewed |
