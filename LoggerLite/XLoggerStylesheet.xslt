<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="html"  encoding="utf-8"/>
  <xsl:template match="activity">
    <head>
      <title>Log</title>
      <style type="text/css">
        body{ text-align: left; font-family: Verdana, sans-serif; }
        table{ border: none;  border-collapse: separate;  width: 100%; }
        tr.title td{ font-size: 24px;  font-weight: bold; }
        th{ background: #d0d0d0;  font-weight: bold;  font-size: 10pt;  text-align: left; }
        tr{ background: #eeeeee}
        td, th{ font-size: 8pt;  padding: 1px;  border: none; }
        tr.info td{}
        tr.warning td{ background-color:yellow; color:black }
        tr.error td{ background-color:red; color:black }
        span{ text-decoration:underline }
      </style>
    </head>

    <body>
      <table>
        <tr class="title">
          <td colspan="7">Log</td>
        </tr>
        <tr>
          <td colspan="1">informations</td>
          <td colspan="6">
            <xsl:value-of select="count(entry[type='Information'])"/>
          </td>
        </tr>
        <tr>
          <td colspan="1">warnings</td>
          <td colspan="6">
            <xsl:value-of select="count(entry[type='Warning'])"/>
          </td>
        </tr>
        <tr>
          <td colspan="1">errors</td>
          <td colspan="6">
            <xsl:value-of select="count(entry[type='Error'])"/>
          </td>
        </tr>
        <tr>
          <th >Time</th>
          <th >Type</th>
          <th >Description</th>
        </tr>
        <xsl:apply-templates/>
      </table>
    </body>
  </xsl:template>

  <xsl:template match="entry">
    <tr id="{type}" class="{type}">
      <td width="20%" id="time">
        <xsl:value-of select="time"/>
      </td>
      <td width="10%" id="type">
        <xsl:value-of select="type"/>
      </td>
      <td  id="description">
        <xsl:value-of select="description"/>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>