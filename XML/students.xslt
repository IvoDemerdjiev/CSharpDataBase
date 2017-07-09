<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:s="urn:students" exclude-result-prefixes="s"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/">
        <ul>
            <xsl:apply-templates 
              select="s:students/s:student"/>
        </ul>
    </xsl:template>

  <xsl:template match="s:student">
    <li>
      <xsl:apply-templates
        select="."/>
    </li>
  </xsl:template>
  
</xsl:stylesheet>
