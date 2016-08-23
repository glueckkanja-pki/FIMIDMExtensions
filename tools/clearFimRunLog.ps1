<#
	.Synopsis
	Clears the list of runs for FIM 2010

	.Description
	The list of runs for FIM 2010 does not get cleared by default. The longer the list becomes, the longer it takes to switch to the Operations tab in the FIM Synchronization Service Manager. Longer lists also lead to database growth and performance degradation, as described here: http://social.technet.microsoft.com/wiki/contents/articles/7034.how-to-clear-the-run-history.aspx
		
	.Example
	clearFIMRunLog -targetcomputer localhost -saveLogPath c:\log\fimlog-
	Stores all entries in the run log in a file and clears the log for FIM running on localhost

	.Parameter targetcomputer
	Specify the name computer running the FIM Synchronization Service
	
	.Parameter saveLogPath
	Specify where to save the run log before it is cleared. The current date and extension are automatically added

	.Parameter dateClear
	Up until which day (excluding the given day) shall the logs be cleared? Only whole days can be specified. Time values on that day will be ignored.
	
	.Notes
	NAME: clearFIMRunLog
	AUTHOR: Christoph Hannebauer of Glueck & Kanja Consulting AG
	LASTEDIT: 2012-05-25
	KEYWORDS: FIM 2010, Operations, Run Log

	#Requires -Version 2.0
#>
param([string]$targetcomputer="localhost",[string]$saveLogPath=$null,[DateTime]$dateClear=(get-date))

$dateClear = $dateClear.Subtract($dateClear.TimeOfDay);
$sDateClear = $dateClear.ToString("yyyy'-'MM'-'dd");

if ($saveLogPath) {
	# Save Run History to log path
	$saveLogFile = $saveLogPath + $sDateClear + ".xml";
	$runHistoryComplete = @(get-wmiobject -class "MIIS_RunHistory" -namespace "root\MicrosoftIdentityIntegrationServer" -ComputerName $targetcomputer);
	$xmlHistoryBuilder = [xml]"<run-history></run-history>";
	$historyRootNode = (Select-Xml -Xpath "/run-history" $xmlHistoryBuilder).Node;
	foreach($historyObject in $runHistoryComplete | where {[DateTime]$_.RunEndTime -lt $dateClear}) {
		$currentXmlHistory = [xml]$historyObject.RunDetails().ReturnValue;
		$nextHistoryNodeSrc = (Select-Xml -Xpath "/run-history/run-details" $currentXmlHistory).Node;
		$nextHistoryNodeDst = $xmlHistoryBuilder.ImportNode($nextHistoryNodeSrc, $true);
		$dummy = $historyRootNode.AppendChild($nextHistoryNodeDst);
	}
	$xmlHistoryBuilder.Save($saveLogFile);
	Write-Output ("Saved Run History until " + $sDateClear.ToString() + " to " + $saveLogFile);
}

# Clear Run History
$fimServer = get-wmiobject -class "MIIS_Server" -namespace "root\MicrosoftIdentityIntegrationServer" -ComputerName $targetcomputer;

Write-Output ("Clear Run History until " + $sDateClear);
$clearResult = $fimServer.ClearRuns($sDateClear);
Write-Output ("Clear result: " + $clearResult.ReturnValue);