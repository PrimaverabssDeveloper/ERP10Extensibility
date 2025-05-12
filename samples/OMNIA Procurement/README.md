# OMNIA Procurement

OMNIA Procurement - Extensibility allows you to communicate Requisition and Purchasing documents to the OMNIA Portal.

(The Omnia Procurement product is discontinued as of SR12. https://helpcenter.ila.cegid.com/v10/novidades/?news_id=135311#logistica)


To use the Procurement template, it is necessary to Import and Configure the Procurement template

Follow the steps here to Import template and configure:
https://developers.ila.cegid.com/v10/recursos/omnia-procurement/

Templates in the Templates folder


Register the assembly as described in: 
https://developers.ila.cegid.com/en/v10/resources/reference/article/how-to-register-extensibility-projects/


To create an entry for the Form frmParametrosOMNIA in User Menus as described in : 
https://developers.ila.cegid.com/en/v10/resources/reference/article/how-to-register-a-user-form/
https://developers.ila.cegid.com/en/v10/resources/reference/article/how-to-create-user-menus/

In the case of this form, you must choose Extensibility Macro and not User Form.


To configure OMNIA parameters, follow these steps:

Step 1: Configure parameters in OMNIA
        - Access the OMNIA subscription in the Management menu | APIClients | Add New;
        - Save the Client ID and the Client Secret generated;
        - Access the OMNIA tenant in the Settings | Definitions | PRIMAVERA ERP;
        - Save the OMNIA Database Suffix.

Step 2: Configure parameters in the ERP
        - Open the User Menu and open the Form Parametros OMNIA;
        - Fill in the following fields:
        - Endpoint (example: https://xxxxxx.primaveraspace.com/);
        - Endpoint API (example: /api/v1/ );
        - Endpoint Identity (example: /identity/connect/token);
        - Tenant (tenant code);
        - Omnia BD (OMNIA Database Suffix);
        - ClientID;
        - Client Secret;


Note: User fields are created automatically, there may be a need to Rebuild All Dependencies.
            - Access the Administration menu | User Fields and, with the right mouse button, 
              select the Rebuild All Dependencies option.

