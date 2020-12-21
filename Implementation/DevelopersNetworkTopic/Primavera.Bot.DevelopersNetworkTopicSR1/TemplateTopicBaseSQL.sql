/*
+-----------------------------------------------------------------------------------------------------------------------------------+
| Query description:	Create new topic																							|
+-----------------------------------------------------------------------------------------------------------------------------------+
| Table:				BotTopics																									|
| Description:			Where the base information for all topics are stored														|
+-----------------------------------+----------------------------------+-----------------------------+------------------------------+
|            NaturalKey             |          DescriptionId           |          CreatedBy          |          ModifiedBy          |
+-----------------------------------+----------------------------------+-----------------------------+------------------------------+
| This element identifies the topic | The description ID for the topic | User that created the topic | User that modified the topic |
| and is contained in bot artifacts | (used for translation)           |                             |                              |
| throughout the application        |                                  |                             |                              |
+-----------------------------------+----------------------------------+-----------------------------+------------------------------+
| SQL managed/auto generated columns																								|
+-----------------------------------+----------------------------------+-----------------------------+------------------------------+
|				Id					|			  CreatedOn			   |		 ModifiedOn			 |			RowVersion			|
+-----------------------------------+----------------------------------+-----------------------------+------------------------------+
| Item unique identifier			| Item creation date			   | Item modification date		 | Timestamp value				|
+-----------------------------------+----------------------------------+-----------------------------+------------------------------+
*/
INSERT INTO Bot.BotTopics(NaturalKey, DescriptionId, CreatedBy, ModifiedBy)
VALUES('DevelopersNetworkTopic', 'Topic_DevelopersNetwork', 'MyUser', 'MyUser')

/*
+------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| Query description:	Create a new task (linked to the new template topic)																							 |
+------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| Table:				BotTasks																																		 |
| Description:			Where the base information for all tasks are stored																								 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
|				NaturalKey					|				DescriptionId			|			   TopicId				|                  Importance                    |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
| This element identifies the topic and is 	| The description ID for the topic		| Parent topic Identifier			| This element identifies the topic importance   |
| contained in bot artifacts throughout the	| (used for translation)				|									| which is one of the items that factors the     |
| application								|										|									| message priority. Values are 0, 1 and 2        |
|											|										|									| from low to high importance                    |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
|              ScheduleConfig				|              MessageConfig            |           PlatformsConfig			|				  PipelineConfig				 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
| Contains all the information about the    | Contains important information about	| Contains information about a		| Contains all pipeline information for the		 |
| schedulling of a task						| the message scope                     | supported platform                | tasks in this file. Each pipeline has a		 |
|											|										|									| sequence of handlers and an Id that can be	 |
|											|										|									| mapped as the entry point for a task execution.|
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
|					System					|				  Active				|			   CreatedBy			|					ModifiedBy					 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
| If task is system owned/created			| If task is active						| User that created the task		| User that modified the task					 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+	
|         AllowConfigReceivers				|								ReceiverOption								|					 Version					 |
+-------------------------------------------+---------------------------------------------------------------------------+------------------------------------------------+
| If the task allows configuration for		| Type of receiver selected in the task										| Task version (system controlled)				 |
| messages receivers						| Values are:																|												 |
|											| 0 (None)																	|												 |
|											| 1 (All users in current instance)											|												 |
|											| 2 (All users in current company)											|												 |
|											| 3 (Only the author)														|												 |
|											| 4 (List of selected users, profiles and/or profile operations)			|												 |
+-------------------------------------------+---------------------------------------------------------------------------+------------------------------------------------+
| SQL managed/auto generated columns																																	 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
|					 Id						|			    CreatedOn				|			  ModifiedOn			|					RowVersion					 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
| Item unique identifier					| Item creation date					| Item modification date			| Timestamp value								 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
| Other columns in table (auto filled when task is configured)																											 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
|				  CompanyId					|			  ToleranceConfig			|		 ResilienceConfig			|				NotificationConfig				 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+
| Company identification code, set when		| Configuration for task execution		| Configuration for task execution	| Configuration for failure notifications		 |
| task is changed							| tolerance (ex: executes task at 		| resilience (ex: repeat execution	| (ex: notify system administrators on last fail)|
|											| 15:30h; executes until Y hour limit)	| after 30 minutes until it fails	|												 |
|											|										| 5 times)							|												 |
+-------------------------------------------+---------------------------------------+-----------------------------------+------------------------------------------------+	
*/
DECLARE @TopicId INT

-- Gets the topic unique identifier (auto generated by SQL) based on its natural key
SET @TopicId = (SELECT TOP 1 Id FROM Bot.BotTopics WHERE NaturalKey = 'DevelopersNetworkTopic')

INSERT INTO Bot.BotTasks(NaturalKey, DescriptionId, TopicId, Importance, ScheduleConfig, MessageConfig, PlatformsConfig, PipelineConfig, [System], Active, CreatedBy, ModifiedBy, AllowConfigReceivers, ReceiverOption, [Version])
VALUES('GenerateTemplateMessages', 'Task_GenerateTemplateMessages', @TopicId, 1,
/*	+ ScheduleConfig (JSON format)------------------------------------------------------+ 
	| Active: If the schedule is active													| 
	+ ----------------------------------------------------------------------------------+
	| Execute: This element identifies the time unit for task execution.				|
	| Values are Daily, Monthly, Weekly.												|
	| Execution takes place ONCE every time unit.										| 
	+ ----------------------------------------------------------------------------------+
	| Active: If the schedule is active													| 
	+ ----------------------------------------------------------------------------------+
	| StartAt: The start hour, month day or week day for each time unit.				| 
	| 0-24 for "Daily", 1-31 for "Monthly" and 1-7 for "Weekly" (0 = Sunday).			|
	+ ----------------------------------------------------------------------------------+
	| StartTolerance: The ammount of hours/days/week days the task is allowed			|
	| to execute if it hasn't executed yet for that time unit.							|
	+ ----------------------------------------------------------------------------------+	*/
'{"Active":true,"Execute":""Monthly"","StartAt":25,"StartTolerance":5}',
/*	+ MessageConfig (JSON format)-------------------------------------------------------+
	| Scope: How the system should generate messages.									|
	| Values Are "Instance", "Instance|Enterprise", "Instance|User"						|
	| and "Instance|Enterprise|User" which means the system will generate one			|
	| message per Instance, one messages per Instance User, one message per				|
	| Instance enterprise/company and finally one message per combination of			|
	| instance, enterprises/companies and users.										|
	+ ----------------------------------------------------------------------------------+
	| ExpireDays: Number of days generated messages are valid.							|
	+ ----------------------------------------------------------------------------------+	*/
'{"Scope":"Instance|Enterprise|User","ExpireDays":365}',
/*	+ PlatformsConfig (XML format)------------------------------------------------------+ 
	| Version: Product version to support. Value is V100 only							|
	+ ----------------------------------------------------------------------------------+
	| platform: Product line to support. Values are "Executive" and	"Professional".		|
	+ ----------------------------------------------------------------------------------+	*/
'{"Version":"V100", "platform":"Executive"}',
/*	+ PipelineConfig (JSON format)------------------------------------------------------+ 
	| Each handler can have its configuration string, that can contain certain			|
	| tokens that adapt it to the execution context when they are schedulled			|
	| for execution.																	|
	+ ----------------------------------------------------------------------------------+
	| %%InstanceId%% - Identifies the instance the task will be executed on.			|
	| Ex: V100|Executive|Default														|
	| %%UserFilter%% - Identifies the user the task will be executed for.				|
	| Use if message scope contains "User".												|
	| %%EnterpriseFilter%% - Identifies the company the task will be executed for.		|
	| Use if message scope contains "Enterprise"										|
	| %%TenantId%% - Identifies the tenant (license id) the task will be executed for.	|
	| %%OrganizationId%% - Identifies the organization the task will be executed for.	|
	| %%TopicId%% and %%TaskId%% - Used to track the topic id and task id the handler	|
	| is being executed on.																|
	+ Handlers example------------------------------------------------------------------+	
	| ErpReadConfigHandler: This builtin handler reads all supported ERP instances		|
	| from the system.																	|
	| TemplateHandler: This is the place where the topic handler(s) must be placed.		|
	| CreateUserBotMessagesHandler: This builtin handler gets a list of users and		|
	| demultiplies messages generated by the task										|
	| SaveBotMessagesHandler: This builtin handler commits all messages processed		|
	| to the database.																	|
	+ ----------------------------------------------------------------------------------+	*/
'<Pipeline Id="DevelopersNetworkTopicPipeline">
	<Handlers>
		<Handler Id="ErpReadConfigHandler" Order="1" Behavior="Reader" Type="Primavera.Platform.HurakanHandlers.ErpReadConfig" ConfigStr="instanceIdFilter=%%InstanceId%%;userFiltler=%%UserFilter%%;enterpriseFilter=%%EnterpriseFilter%%"/>
		<Handler Id="TemplateHandler" Order="2" Behavior="Reader" Type="Primavera.Bot.DevelopersNetworkTopic.TemplateHandler" ConfigStr="topicId=%%TopicId%%;taskId=%%TaskId%%"/>
		<Handler Id="CreateUserBotMessagesHandler" Order="3" Behavior="Reader" Type="Primavera.Hurakan.BotHandlers.CreateUserBotMessages" ConfigStr=""/>
		<Handler Id="SaveBotMessagesHandler" Order="4" Behavior="Reader" Type="Primavera.Hurakan.BotHandlers.SaveBotMessages" ConfigStr=""/>
	</Handlers>
</Pipeline>',
0, 1, 'MyUser', 'MyUser', 1, -1, 1)

/*
+-------------------------------------------------------------------------------------------------------------------------------------------------------+
| Query description:	Create translation data																											|
+-------------------------------------------------------------------------------------------------------------------------------------------------------+
| Table:				BotStringTable																													|
| Description:			Where the translation texts, by language (PT and/or EN), for both topics and tasks are stored									|
+------------------------------------+----------------------------------+------------------------------+-----------------------+------------------------+
|             ResourceId             |             Language             |             Text             |       CreatedBy       |       ModifiedBy       |
+------------------------------------+----------------------------------+------------------------------+-----------------------+------------------------+
| Unique identifier for the resource | Language in which the text will  | The translated text based on | User that created the | User that modified the |
| (same one set in "BotTopics" and   | be translated (pt-pt, en-en)		| language                     | translation           | translation            |
| "BotTasks" tables)                 |                                  |                              |                       |                        |
+------------------------------------+----------------------------------+------------------------------+-----------------------+------------------------+
| SQL managed/auto generated columns																						   |
+------------------------------------+----------------------------------+------------------------------+-----------------------+
|				 Id					 |			   CreatedOn			|			ModifiedOn		   |	   RowVersion	   |
+------------------------------------+----------------------------------+------------------------------+-----------------------+
| Item unique identifier			 | Item creation date				| Item modification date	   | Timestamp value	   |
+------------------------------------+----------------------------------+------------------------------+-----------------------+
*/
-- Portuguese
INSERT INTO Bot.BotStringTable(ResourceId, [Language], [Text], CreatedBy, ModifiedBy)
VALUES('Topic_DevelopersNetwork', 'pt-pt', 'Tópico de Template Developers Network', 'MyUser', 'MyUser')
INSERT INTO Bot.BotStringTable(ResourceId, [Language], [Text], CreatedBy, ModifiedBy)
VALUES('Task_GenerateTemplateMessages', 'pt-pt', 'Geração de mensagens exemplo', 'MyUser', 'MyUser')
-- English
INSERT INTO Bot.BotStringTable(ResourceId, [Language], [Text], CreatedBy, ModifiedBy)
VALUES('Topic_DevelopersNetwork', 'en-en', 'Developers Network Template Topic', 'MyUser', 'MyUser')
INSERT INTO Bot.BotStringTable(ResourceId, [Language], [Text], CreatedBy, ModifiedBy)
VALUES('Task_GenerateTemplateMessages', 'en-en', ' Generation of example messages', 'MyUser', 'MyUser')