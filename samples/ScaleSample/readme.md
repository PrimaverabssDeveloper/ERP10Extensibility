# Interface to obtain weights from various scales on the market

#### At Sales | Resources | Scales must be provided the name of the assembly generated.

1. Add a new reference to VndBE100
2. Create a new class, that must inherit from VndBE100.IProxyBalanca
3. This interface have the following implementations


## Properties

| Name        | Description
| ----------- | -----------
| Status      | Scale status: You can take the values: <P>stable = 0, unstable = 1, invalid = 3</P>
| UltimoErro  | Number of the last error that occurred.

## Methods

| Name				| Description
|------------		| -----------------------------------------------------------------------------------------------
| Config			| <p>Configures the parameters of the balance.</P> <P>For example, can be used to define the parameterization of the serial port and the communication protocols to be used. </p>
| DaDescritivoErro	| Returns the description of the last error occurred.
| DaPeso         	| <p>It reads the weight at the moment. <P></P>nRet will have immediate weight (not stable scale) <P></P>Returns TRUE if the weight is obtained successfully. </P><P>FALSE otherwise.</p>
| DaPesoEstavel     | <p>Makes the weight reading stable.</p><p>It has the same behavior as the previous function, except that the weight returned is stable.</p><p>tAmost = Sampling time for the weight to be considered stable in ms. This parameter is optional. Typically set to 3s (3000ms)</p><p>Some scales even have a flag that indicates that the weight is stable. In this case, this parameter must be ignored, and the balance flag must be used to obtain the stable weight.</p>
| Inicializa     	| <p>Initialize the scale.</p><p>It is always invoked before any balance function is used.</p><p>Post = Post code of the POS.</p><p>Path is optional. This must be the application path (POS). It is essentially used to generate LOG files to control the DLL itself.</p>
| Termina          	| <p>Stop using the scale.</p><p>All operations necessary to finish using the scale are placed here.</p><p>Returns TRUE if it succeeds.</p>